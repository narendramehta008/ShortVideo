import React, { useEffect } from 'react'
import "../styles/standard.scss";
interface IProps {
    title: string;
    description: string;
    cardImageUrl: string;
}

export const NeuCard: React.FC<IProps> = ({ title, description, cardImageUrl }) => {

    useEffect(() => {
    }, [])

    return (
        <div className="shadow border-sm" style={{ marginBottom: '1rem' }}>
            <div className="card-top-p">
                <img className=" border-sm img" src={cardImageUrl} alt="demo" />
            </div>
            <div className="card-body">

                <h2 className="card-title ">{title}</h2>

                <p>{description}</p>
                <div style={{ textAlign: 'right' }}>
                    <a className="btn btn-link btn-primary border" style={{ marginTop: '1rem' }} href="">MORE</a>
                </div>

            </div>
        </div>

    )
}
