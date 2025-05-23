import React, { FC, PropsWithChildren, useEffect } from 'react';
import { useSheshaApplication } from '@shesha-io/reactjs';
import { formDesignerComponents } from '../../designer-components';
//import { formDesignerComponents } from '../designer-components';

export interface IImageScanPluginProps {

}

export const REPORTING_PLUGIN_NAME = 'ImageScan';

export const ImageScanPlugin: FC<PropsWithChildren<IImageScanPluginProps>> = ({ children }) => {
    const { registerFormDesignerComponents } = useSheshaApplication();

    useEffect(() => {
        registerFormDesignerComponents(REPORTING_PLUGIN_NAME, formDesignerComponents);
    }, [registerFormDesignerComponents]);

    return (
        <>{children}</>
    );
};