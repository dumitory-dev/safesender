import '@/styles/globals.css'
import type { AppProps } from 'next/app'
import Script from 'next/script'
import Layout from '../components/layout/layout'

export default function App( { Component, pageProps }: AppProps ) {
  return <>
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
  return
}
