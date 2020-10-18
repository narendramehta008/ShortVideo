import React, { MouseEventHandler } from 'react'

interface IProps {
    className: string;
    onClick: any;
    value: string;
}

export const NeuButton: React.FC<IProps> = ({ className, onClick, value }) => {
    return (
        <button className={className} onClick={(event) => onClick(event)} >{value}</button>
    )
}
