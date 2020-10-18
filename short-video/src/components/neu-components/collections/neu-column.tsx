import React from 'react'

interface IProps {

}
export const NeuColumn: React.FC<IProps> = ({ children }) => {
    return (
        <div className="col">
            {children}
        </div>
    )
}
