import Axios from "axios";
import React, { useEffect, useState } from "react";
import "./auth.scss";
import { Constants } from "../../../shared/models/Constants";
import { NeuFormInput } from "../../../components/neu-components/inputs/neu-form-input";
import { NeuForm } from "../../../components/neu-components/forms/NeuForm";
import { useHistory } from "react-router-dom";


const apis = Constants;

export const CustomLogin = () => {

    useEffect(() => {

    }, []);

    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
    const history = useHistory();

    const [validationStatus, setValidationStatus] = useState([
        { id: "username", value: true },
        { id: "password", value: true },
    ])

    const onSubmit = () => {
        var postData = {
            username: userName,
            password: password
        }
        Axios.post(`${apis.baseUrl}${apis.auth.login}`, postData).then(a => {
            localStorage.setItem('token', a.data.token);
            localStorage.setItem('username', userName);
            window.location.reload();
            //history.push("/");
        }).catch(c => {
            setError(c.response.statusText);
        });
    }

    const validation = (key: string, value: boolean) => {
        var index = validationStatus.findIndex(x => x.id === key);
        let status = [...validationStatus];
        status[index].value = value;
        setValidationStatus(status);
    }

    const validate = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (!userName && !password) {
            setError('Username or password is empty');
            return;
        }
        event.preventDefault();
        validationStatus.map(valid => {
            if (!valid.value)
                return false;
        })
        return true;
    }

    return (
        <NeuForm title='User Login' logoSrc='/logo192.png' subTitle='user' onClick={(event) => validate(event) && onSubmit()} >
            {/* <NewFields>
            </NewFields> */}
            {/* label="username" fieldName="username" */}
            <div className="neu-fields">
                <NeuFormInput inputkey="username" type="username" inputValue={userName} requiredError="Required" onChange={(event) => setUserName(event.target.value)} placeHolder="username" validationFunction={validation} >
                </NeuFormInput>
                <NeuFormInput inputkey="password" type="password"
                    inputValue={password} requiredError="Required" onChange={(event) => { event.preventDefault(); setPassword(event.target.value); }} placeHolder="password" validationFunction={validation}  >
                </NeuFormInput>
            </div>
        </NeuForm>
    );
};
