import React, { useEffect } from 'react';
import styles from './Table.module.css';
import Button from '../Button';
import { useAppSelector } from '../../../store/store';

const Table = ({
  items,
  setItems,
  tableHead,
  tableBody,
  title,
  itemParams,
  isAddMoreDisable,
}) => {
  const isThemeDark = useAppSelector((state) => state.theme.isThemeDark);

  useEffect(() => {
    handleAddUserClick();
  }, []);

  const handleAddUserClick = () => {
    const newItem = {
      id: Date.now(),
      ...itemParams,
    };
    setItems([...items, newItem]);
  };

  return (
    <>
      <div
        className={`${styles.tableDiv} table-container table-class bg-background text-text`}
      >
        <table
          className={`table ${styles.table} bg-background text-text ${
            isThemeDark ? 'table-dark' : ''
          }`}
        >
          <thead>{tableHead}</thead>
          <tbody>{tableBody}</tbody>
        </table>
      </div>
      {isAddMoreDisable ? (
        <Button onClick={handleAddUserClick}>Add new {title}</Button>
      ) : items.length < 1 ? (
        <Button onClick={handleAddUserClick}>Add new {title}</Button>
      ) : (
        ''
      )}
    </>
  );
};

export default Table;
