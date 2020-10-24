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
    fieldName?: string;
    label?: string;
}
interface IRef {
    error: string;
}

export const NeuFormInput: React.FC<IProps> = ({ inputkey, type, fieldName, label, inputValue, onChange, requiredError,
    pattern, patternError, placeHolder, validationFunction }) => {

    const [error, setError] = useState("");
    const textInput = createRef<HTMLDivElement>();
    const validating = (event: ChangeEvent<HTMLInputElement>) => {
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

            <div className={label && fieldName ? "neu-form-input" : "neu-field"}>
                {label && fieldName ? <label className="neu-label" htmlFor={fieldName}>{label}</label> : null}
                <input className={label && fieldName ? "neu-field" : ""}
                    name={fieldName}
                    type={type}
                    value={inputValue}
                    onChange={(event) => {
                        validating(event);
                        onChange(event)
                    }}
                    placeholder={placeHolder}
                />
            </div>
            {error ? <div className='neu-field-error' ref={textInput} >{error}</div> : null}
        </div>
    )
}
