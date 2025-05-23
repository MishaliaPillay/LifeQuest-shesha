import { FormLayout } from 'antd/lib/form/Form';

export const getSettings = () => {

    return {

        components: [],
        formSettings: {
            colon: false,
            layout: 'vertical' as FormLayout,
            labelCol: { span: 24 },
            wrapperCol: { span: 24 }
        }
    };
};