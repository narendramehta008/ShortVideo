import React, { ChangeEvent, ChangeEventHandler, createRef, MutableRefObject, useEffect, useRef, useState } from 'react'
import './inputs.scss';

interface IProps {
    inputkey: string;
    inputValue: string;
    onChange: ChangeEventHandler<HTMLInputElement>;
    requiredError?: string;
    pattern?: string | undefined;
    patternError?: string | undefined;
    type: string;
    isValid?: boolean | true;
    placeHolder?: string;
    validationFunction: (key: string, value: boolean) => void;
}
interface IRef {
    error: string;
}

export const NeuFormInput: React.FC<IProps> = ({ inputkey, type, inputValue, onChange, requiredError, pattern, patternError, placeHolder, validationFunction }) => {

    const [error, setError] = useState("");
    const textInput = createRef<HTMLDivElement>();
    const checking = (event: ChangeEvent<HTMLInputElement>) => {
        if (pattern) {
            var regexConst = new RegExp(pattern);
            if (!regexConst.test(event.target.value)) {
                if (patternError)
                    setError(patternError);
                else
                    setError("Pattern not matches.");
            }
        }
        else setError('');
        if (requiredError && !event.target.value)
            setError(requiredError);
        else setError('');

    }

    useEffect(() => {
        validationFunction(inputkey, error ? false : true);
    }, [error])

    return (
        <div >
            <div className="neu-field" >
                <input
                    type={type}
                    value={inputValue}
                    onChange={(event) => {
                        checking(event);
                        onChange(event)
                    }}
                    placeholder={placeHolder}
                />
            </div>
            {error ? <div className='neu-field-error' ref={textInput} >{error}</div> : null}
        </div>
    )
}
