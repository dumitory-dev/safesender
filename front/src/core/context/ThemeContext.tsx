import React, { useContext, useState } from 'react';

export interface ContextProps {
    darkMode: boolean;
    setDarkMode: any;
}

const ThemeContext = React.createContext<ContextProps>( { darkMode: false, setDarkMode: () => { } } );

export const useTheme = () => {
    return useContext( ThemeContext );
}

export const ThemeProvider = ( { children }: { children: any } ): React.ReactElement => {

    const [ darkMode, setDarkMode ] = useState( false );

    const toggleTheme = ( toggle: boolean ) => {
        setDarkMode( toggle );

        if ( toggle ) {
            document.documentElement.classList.add( 'dark' );
        } else {
            document.documentElement.classList.remove( 'dark' );
        }
    }

    return (
        <ThemeContext.Provider value={{ darkMode, setDarkMode: toggleTheme }}>
            {children}
        </ThemeContext.Provider>
    )

}
