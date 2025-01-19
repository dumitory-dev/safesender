import { createContext, useContext, useState } from 'react';

export interface DataContextProps {
    token: string;
    link: string;
    hasFile: boolean;
    setToken: ( token: string ) => void;
    setHasFile: ( hasFile: boolean ) => void;
    setLink: ( link: string ) => void;
}

const DataContext = createContext<DataContextProps>( { token: '', hasFile: false, link: '', setToken: () => { }, setHasFile: () => { }, setLink: () => { } } );

export const useDataContext = () => {
    return useContext( DataContext );
}

export const DataProvider = ( { children }: { children: any } ): React.ReactElement => {

    const [ token, setToken ] = useState<string>( '' );
    const [ link, setLink ] = useState<string>( '' );
    const [ hasFile, setHasFile ] = useState<boolean>( false );

    const setHasFileHandler = ( state: boolean ) => {
        setHasFile( state );
    }

    const setTokenHandler = ( token: string ) => {
        setToken( token );
    }

    const setLinkHandler = ( link: string ) => {
        setLink( link );
    }

    return (
        <DataContext.Provider value={{ token, link, hasFile, setLink: setLinkHandler, setToken: setTokenHandler, setHasFile: setHasFileHandler }}>
            {children}
        </DataContext.Provider>
    )

}