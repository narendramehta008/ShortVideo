import React from 'react'

interface IProp {

}
export const NewFields: React.FC<IProp> = ({ children }) => {
    return (
        <div className="neu-fields">
            {children}
        </div>
    )
}
