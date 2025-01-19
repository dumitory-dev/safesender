import React from 'react';

import { useFileReader } from '../../../core/context/FileReadContext';

export const StatusBar = (): React.ReactElement => {

    const { progress, isReadingFinished } = useFileReader();

    return (
        <div className='wrapper h-[16px] w-[85%] flex justify-between items-center'>

            <div className="bar relative bg-[#DADADA] overflow-hidden rounded-[16px] w-[90%] h-[8px]">
                {!isReadingFinished && <div
                    className={'progress dark:bg-black bg-blueAcc rounded-[16px] h-full'}
                    style={{ width: `${ progress }%` }}></div>
                }
                {isReadingFinished && <div
                    className={'bg-blueAcc dark:bg-black rounded-[16px] h-full w-1/2 absolute  animate-loading'}></div>}
            </div>

            <div className={`text-[13px] font-bold`}>{progress}%</div>

        </div>
    )
}