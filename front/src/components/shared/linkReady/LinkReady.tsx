import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/router';

import copyIco from './../../../../public/copyIco.svg';
import okIco from './../../../../public/ok.svg';

import { Button } from '../button/Button';

export const LinkReadyBox = ( { extLink }: { extLink: string } ): React.ReactElement => {

    const [ link, setLink ] = useState<string | undefined>( extLink );
    const [ copied, setCopied ] = useState<boolean | undefined>( undefined );

    const router = useRouter();

    const clickHandler = (): void => {
        navigator.clipboard.writeText( link! );
        setCopied( true );
    };

    useEffect( () => {

        if ( copied ) {
            setTimeout( () => {
                setCopied( false );
            }, 3000 );
            
        }

        if ( !copied ) {
            setCopied( undefined )
        }

    }, [ copied ] );

    useEffect( () => {

        if ( !link ) {
            router.push( { pathname: '/' } );
        }

    }, [ link, router ] )

    return (
        <div className='flex flex-col items-center'>

            {
                link &&
                <>
                    <div className='flex flex-col items-center pb-[24px] box-border'>
                        <span className='text-[32px] font-bold'>Exchange Link</span>
                        <span className='text-[18px] text-gray'>Copy the link to share</span>
                    </div>

                    <div className='border relative w-full
            h-auto break-words text-center border-gray text-[18px] font-[500] rounded-[8px] p-[25px] box-border text-gray'>

                        {/* {copied !== undefined && <SmallInfoPanel
                            state={copied ? '1' : '0'}
                        />} */}

                        {link}
                    </div>

                    <div className='pt-[24px] w-full box-border'>

                        {
                            !!copied ?

                            <Button
                              disabled={false}
                              labelText='Copied!'
                              onClickHandler={clickHandler}
                              color='#1DBE19'
                              icon={okIco}
                            />
                            
                            : <Button
                                disabled={false}
                                labelText='Copy to clipboard'
                                onClickHandler={clickHandler}
                                icon={copyIco}
                              />
                      }

                    </div>
                </>
            }

        </div>

    )
}