import React, { FC, PropsWithChildren, useEffect } from 'react';
import { useSheshaApplication } from '@shesha-io/reactjs';
import { formDesignerComponents } from '../../designer-components';
//import { formDesignerComponents } from '../designer-components';

export interface IPhoenixPluginProps {

}

export const REPORTING_PLUGIN_NAME = 'Phoenix';

export const PhoenixPlugin: FC<PropsWithChildren<IPhoenixPluginProps>> = ({ children }) => {
    const { registerFormDesignerComponents } = useSheshaApplication();

    useEffect(() => {
        registerFormDesignerComponents(REPORTING_PLUGIN_NAME, formDesignerComponents);
    }, [registerFormDesignerComponents]);

    return (
        <>{children}</>
    );
};