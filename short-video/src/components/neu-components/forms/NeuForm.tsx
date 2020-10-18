import React, { ReactElement, useEffect, useState } from 'react'
import '../styles/neu.scss'

interface IProps {
    title: string;
    subTitle?: string;
    onClick: any;
    logoSrc?: string;
}

export const NeuForm: React.FC<IProps> = ({ title, subTitle, onClick, logoSrc, children }) => {


    const [error, setError] = useState("");
    const [logoUrl, setLogoUrl] = useState("")
    const [height, setHeight] = useState(650);

    useEffect(() => {
        setLogoUrl(`url("${logoSrc}")`);

        if (logoSrc)
            setHeight(height + 80);
        if (title)
            setHeight(height + 80);
        if (subTitle)
            setHeight(height + 80);

        React.Children.toArray(children).map(children => {
            setHeight(height + 80);
        })
    }, [])

    useEffect(() => {
        if (error)
            setHeight(height + 80);
        else
            setHeight(height - 80);
    }, [error])

    const validation = () => {
        //checking childs have validation or not
        if (children) {
            var tempChild = children as any;
            tempChild.props.children.map((child: ReactElement) => {
                if ((child.props.requiredError || child.props.pattern) && !child.props.inputValue) {
                    setError('Required fields are empty.')
                    return false;
                }
            });
        }

        return true;
    }

    // height: 650px;
    return (
        <div className="neu-login-content">
            <div className="neu-login-div" style={{ height: height }}>

                {error ? <div className='neu-error-message' >{error}</div> : null}
                {logoSrc ? <div className="neu-logo" style={{ background: logoUrl }}></div> : null}
                {title ? <div className="neu-title">{title}</div> : null}
                {subTitle ? <div className="neu-sub-title">{subTitle}</div> : null}

                {children}

                <button className="neu-signin-button" onClick={(event) => validation() && onClick(event)} >Login</button>
                <div className="link">
                    <a href="#">Forgot password?</a> or <a href="#">Sign up</a>
                </div>
            </div>
        </div>
    )
}
