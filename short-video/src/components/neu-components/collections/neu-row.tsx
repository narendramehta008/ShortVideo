import React, { useEffect, useState } from 'react'

interface IProps {
    columnsCount: number;
}
export const NeuRow: React.FC<IProps> = ({ columnsCount, children }) => {

    const [classValue, setClassValue] = useState('');

    useEffect(() => {
        setClassValue(`row row-cols-${columnsCount} row-cols-sm-2 row-cols-xl-4`)
    }, [])

    return (
        <div className={classValue}>
            {children}
        </div>
    )
}
