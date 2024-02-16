import { PropsWithChildren } from 'react';
import Image from 'next/image';

import waves from '../../../public/waves.svg';
import { Footer } from '../shared/footer/Footer';
import { Header } from '../shared/header/Header';
import { ThemeProvider } from '../../core/context/ThemeContext';
import { LoaderProvider } from '../../core/context/LoaderContext';
import ChildrenWrapper from '../shared/childrenWrapper/childrenWrapper';
import { DataProvider } from '../../core/context/DataContext';
import ErrorDialog from '../shared/errorDialog/ErrorDialog';
import { ErrorProvider } from '../../core/context/ErrorContext';
import Head from 'next/head';


export default function Layout( { children }: PropsWithChildren ) {



    return (
        <>

            <Head>
                <title>SafeSender</title>
                <meta name="description" content="Generated by create next app" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/logo2.svg" />
            </Head>

            <ThemeProvider>

                <LoaderProvider>

                    <DataProvider>
                        
                        <ErrorProvider>

                            <ErrorDialog/>

                            <Header />

                            <main className='h-[calc(100vh-102px)] sm:h-[calc(100%-102px)] overflow-y-auto bg-blue flex justify-center relative dark:bg-black overflow-hidden'>

                                <Image
                                    className='bg sm:hidden select-none w-full max-h-[calc(100vh-100px)] absolute top-[-50px] left-0 right-0 bottom-0'
                                    src={waves}
                                    priority={true}
                                    alt='bg' />

                                <div className='h-[922px] sm:h-auto bigDesktop:h-[1541px] bigDesktop:w-[1191px] flex items-center justify-center relative'>

                                    <ChildrenWrapper>{children}</ChildrenWrapper>

                                </div>

                            </main>

                            <Footer />

                        </ErrorProvider>

                    </DataProvider>

                </LoaderProvider>

            </ThemeProvider>
        </>
    )
}