import React from 'react';
import styles from "./Table.module.css";
import Button from "../Button";

const Table = ({ 
    items, 
    setItems, 
    tableHead, 
    tableBody, 
    title,
    itemParams,
    isAddMoreDisable
}) => {


    const handleAddUserClick = e => {
        e.preventDefault();
        const newItem = {
            id:Date.now(),
            ...itemParams
        };
        setItems([...items, newItem]);
    }

    return (
        <>
            <div className={`${styles.tableDiv} table-container table-class`}>
                <table className={`table ${styles.table}`}>
                    <thead>
                        {tableHead}
                    </thead>
                    <tbody>
                        {tableBody}
                    </tbody>
                </table>
            </div>
            {
                isAddMoreDisable
                ? <Button onClick={handleAddUserClick}>Add new {title}</Button>
                : items.length < 1
                    ? <Button onClick={handleAddUserClick}>Add new {title}</Button>
                    : ""
            }
        </>
    );
};

export default Table;
