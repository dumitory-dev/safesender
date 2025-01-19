import React from 'react';
import Image from 'next/image';

import loaderIco from '../../../../public/loader.png';

export const Loader = (): React.ReactElement => {
    return (
        <div className='absolute z-[99999] top-0 right-0 bottom-0 left-0 backdrop-blur-sm bg-opacity-30 bg-gray-light flex'>
            <Image className='animate-spin m-auto' src={loaderIco} alt={'loader'} />
        </div>
    )
}