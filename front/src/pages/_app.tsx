import React, { Component } from 'react';
import Layout from '../components/layout/layout';
import Script from 'next/script';
import { AppProps } from 'next/app';
import '@/styles/globals.css';

export default function App( { Component, pageProps }: AppProps ) {
  return (
    <>
      <Script strategy="afterInteractive" src="https://www.googletagmanager.com/gtag/js?id=G-P3ZWGZ54ZN" />
      <Script
        id='google-analytics'
        strategy="afterInteractive"
        dangerouslySetInnerHTML={{
          __html: `
          window.dataLayer = window.dataLayer || [];
          function gtag(){dataLayer.push(arguments);}
          gtag('js', new Date());
          gtag('config', 'G-P3ZWGZ54ZN', {
            page_path: window.location.pathname,
          });
        `,
        }} />

      <Layout>
        <Component {...pageProps} />
      </Layout>
    </>
  )
}