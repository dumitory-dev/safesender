import styles from './ThemeToggle.module.css';

export interface ToggleProps {
    darkMode?: boolean;
    clickHandler: any;
}

export const ThemeToggle = ( { darkMode, clickHandler }: ToggleProps ) => {
    return (
        <div className="theme-switcher cursor-pointer" onClick={clickHandler}>
            <svg width="36" height="24" viewBox="0 0 36 24" fill="none" xmlns="http://www.w3.org/2000/svg">

                <rect className={styles.whiteBorder} x="0.5" y="0.5" width="35" height="23" rx="11.5" />
                <g className={!darkMode ? styles.darkDisable : styles.darkEnable}>
                    <rect
                        width="24"
                        height="24"
                        rx="12"
                        className={styles.darkCircle} />
                    <g
                        className={!darkMode ? styles.sunActive : styles.sunDisabled}
                        clipPath="url(#clip0_28_112)">
                        <path className={!darkMode ? styles.sunActive : styles.sunDisabled} d="M12.0003 16C10.9395 16 9.92204 15.5786 9.1719 14.8284C8.42175 14.0783 8.00033 13.0609 8.00033 12C8.00033 10.9392 8.42175 9.92174 9.1719 9.17159C9.92204 8.42145 10.9395 8.00002 12.0003 8.00002C13.0612 8.00002 14.0786 8.42145 14.8288 9.17159C15.5789 9.92174 16.0003 10.9392 16.0003 12C16.0003 13.0609 15.5789 14.0783 14.8288 14.8284C14.0786 15.5786 13.0612 16 12.0003 16ZM11.3337 4.66669H12.667V6.66669H11.3337V4.66669ZM11.3337 17.3334H12.667V19.3334H11.3337V17.3334ZM6.34366 7.28602L7.28633 6.34335L8.70033 7.75735L7.75766 8.70002L6.34366 7.28669V7.28602ZM15.3003 16.2427L16.243 15.3L17.657 16.714L16.7143 17.6567L15.3003 16.2427ZM16.7143 6.34269L17.657 7.28602L16.243 8.70002L15.3003 7.75735L16.7143 6.34335V6.34269ZM7.75766 15.3L8.70033 16.2427L7.28633 17.6567L6.34366 16.714L7.75766 15.3V15.3ZM19.3337 11.3334V12.6667H17.3337V11.3334H19.3337ZM6.66699 11.3334V12.6667H4.66699V11.3334H6.66699Z" fill="white" />
                    </g>

                    <g className={!darkMode ? styles.moonDisabled : styles.moonActive}
                        clipPath="url(#clip0_28_156)">
                        <path d="M7.58634 1.34601C7.0951 1.80376 6.70108 2.35576 6.4278 2.96909C6.15452 3.58242 6.00758 4.24451 5.99573 4.91586C5.98389 5.58721 6.10738 6.25407 6.35886 6.87666C6.61033 7.49925 6.98463 8.06481 7.45942 8.5396C7.93421 9.01439 8.49977 9.38869 9.12236 9.64016C9.74495 9.89163 10.4118 10.0151 11.0832 10.0033C11.7545 9.99144 12.4166 9.84449 13.0299 9.57121C13.6433 9.29794 14.1953 8.90392 14.653 8.41268C14.441 11.9027 11.5437 14.6667 8.00034 14.6667C4.31767 14.6667 1.33301 11.682 1.33301 8.00001C1.33301 4.45667 4.09701 1.55934 7.58634 1.34601Z" fill="#070707" />
                    </g>
                    <defs>
                        <clipPath id="clip0_28_156">
                            <rect width="16" height="16" fill="white" />
                        </clipPath>
                    </defs>

                    <defs>
                        <clipPath id="clip0_28_112">
                            <rect width="16" height="16" fill="white" transform="translate(4 4)" />
                        </clipPath>
                    </defs>
                </g>
            </svg>
        </div>
    )
}