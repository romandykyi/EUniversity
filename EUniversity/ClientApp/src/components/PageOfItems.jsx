import React from 'react';
import PaginatedList from './PaginatedList';

const PageOfItems = ({
    title,
    fetchFunction,
    isLoading,
    itemsPerPage,
    setItemsPerPage,
    totalItems,
    tableHead,
    tableBody,
    additionalItems,
    usersType
}) => {
    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
                {title}
            </h1>
            {additionalItems}
            <PaginatedList
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                isLoading={isLoading}
                totalItems={totalItems}
                fetchItems={fetchFunction}
                tableHead={tableHead}
                tableBody={tableBody}
                usersType = {usersType}
            />
        </div>

    );
};

export default PageOfItems;