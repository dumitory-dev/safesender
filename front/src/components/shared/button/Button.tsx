import React from 'react';
import Image from 'next/image';

export interface ButtonProps {
    disabled?: boolean;
    icon: any;
    labelText: string;
    color?: string;
    onClickHandler: () => void;
}

export const Button = ( { onClickHandler, disabled, icon, color, labelText }: ButtonProps ): React.ReactElement => {
    return (
        <button
            onClick={onClickHandler}
            disabled={disabled}
            className={`dark:bg-black bg-blue w-full text-white border-[1px] rounded-[8px] h-[64px] flex items-center justify-center text-[18px] font-bold dark:text-white ${ disabled && 'opacity-50 cursor-not-allowed' }`}
            style={{backgroundColor: color}}>
            <Image className='mr-[8.5px]' src={icon} alt='share' />
            {labelText}
        </button>
    )
}