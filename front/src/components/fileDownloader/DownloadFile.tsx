import Image from 'next/image';
import React, { useEffect, useState } from 'react';
import { decrypt_async } from 'wasm-xchacha20-poly';
import { decode } from "@msgpack/msgpack";

import loaderIco from '../../../public/loader.png';
import downLoadIcon from '../../../public/download.svg';

import { Button } from '../shared/button/Button';
import { caesar } from '../../core/helpers/caesar';
import { FileItem } from '../shared/fileItem/FIleItem';
import { saveByteArrayAsFile } from '../../core/helpers/saveFile';
import { PasswordInput } from '../shared/passwordInput/PasswordInput';
import { getHashPassword } from '../../core/helpers/getHashPassword';
import { fixPasswordLenIfNeeded } from '@/core/helpers/fixPasswordLenIfNeeded';
import { CORRECT_PASSWORD_LENGTH } from '../../core/configs/password-length';
import { InternalApiDownloadResponse } from '../../models/internal-api';
import { Downloaded } from './downloaded/Downloaded';


export interface DownloadFileProps {
    readonly token: string;
}

export const DownloadFile = ( { token }: DownloadFileProps ): React.ReactElement => {

    const [ password, setPassword ] = useState<string>( '' );
    const [ file, setFile ] = useState<File | undefined>();
    const [ encryptedFile, setEncryptedFile ] = useState<any>();
    const [ fileName, setFileName ] = useState<string>( '' );
    const [ fileSize, setFileSize ] = useState<number>();
    const [ originalPasswordHash, setOriginalPasswordHash ] = useState<string>( '' );
    const [ isCorrectPassword, setIsCorrectPassword ] = useState<boolean>( false );
    const [ downloaded, setDownloaded ] = useState<boolean>( false );


    useEffect( () => {

        const loadFile = async () => {

            if ( token ) {

                try {

                    const response = await fetch( `${ process.env.NEXT_PUBLIC_INTERNAL_API }download/${ token }` );

                    const internalApiResponse = decode<InternalApiDownloadResponse>( await response.arrayBuffer() );

                    convertFile( internalApiResponse as InternalApiDownloadResponse );

                } catch ( e ) {
                    console.log( 'unknown download error!' );
                }
            }
        }

        loadFile().catch( err => {
            console.log( 'DOWNLOAD_FILE_ERROR:', err );
        } );

    }, [ token ] );


    const convertFile = ( internalApiResponse: InternalApiDownloadResponse ) => {

        const fileNameArray = internalApiResponse.FileName.split( '.' );

        const fileNamePart = internalApiResponse.FileName
            ? ( fileNameArray[ 0 ] || '' )
            : '';

        const fileExtPart = internalApiResponse.FileName
            ? ( fileNameArray?.pop() || '' )
            : '';

        // set state parts
        setFile( new File( [], `${ caesar.decipher( fileNamePart, 5 ) }.${ fileExtPart }` ) );

        setEncryptedFile( internalApiResponse.FileData );

        setFileName( `${ caesar.decipher( fileNamePart, 5 ) }.${ fileExtPart }` );

        setFileSize( internalApiResponse.OriginalFileSize );

        setOriginalPasswordHash( internalApiResponse.PasswordHash );
    }

    const passwordIsIncorrect = ( password: string ): boolean => {
        return originalPasswordHash !== getHashPassword( password );
    }

    const clickHandler = async (): Promise<void> => {

        const encoder = new TextEncoder();
        const finalPassword = fixPasswordLenIfNeeded( password, CORRECT_PASSWORD_LENGTH );
        const encryptKey = encoder.encode( finalPassword );

        const decryptedFile = await decrypt_async( encryptedFile, encryptKey );

        saveByteArrayAsFile( decryptedFile, fileName );

        setDownloaded( true );
    }

    const checkEnteredPassword = ( password: string ) => {

        if ( !password ) {
            return;
        }

        if ( passwordIsIncorrect( password ) ) {
            alert( 'Password is incorrect! Please check your password and try again' );
            setIsCorrectPassword( false );
            return;
        }

        setPassword( password );
        setIsCorrectPassword( true );
    }

    return (

        <>
            {
                downloaded
                    ? <Downloaded />
                    : <div className='flex flex-col items-center'>

                        <div className='flex flex-col items-center pb-[24px] box-border'>
                            <span className='text-[32px] font-bold'>Download</span>
                            <span className='text-[18px] text-gray'>Enter password to download files</span>
                        </div>

                        <div className=''>
                            <PasswordInput
                                hasGenerateToggle={false}
                                password={password}
                                setPassword={checkEnteredPassword} />
                        </div>

                        <div className='w-full'>
                            {
                                file
                                    ? <FileItem
                                        file={file!}
                                        fileSize={fileSize!}
                                        isBlured={!isCorrectPassword}
                                        isDeletable={false}
                                        deleteFile={false} />
                                    : <Image
                                        className='animate-spin m-auto'
                                        src={loaderIco}
                                        alt='loader' />
                            }
                        </div>

                        <div className='pt-[24px] w-full box-border'>
                            <Button
                                disabled={!password}
                                labelText='Download'
                                onClickHandler={clickHandler}
                                icon={downLoadIcon} />
                        </div>
                    </div>
            }
        </>
    )
}
