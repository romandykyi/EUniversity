import React from 'react';
import PaginatedList from './PaginatedList';

const PageOfItems = ({
    title,
    isLoading,
    itemsPerPage,
    setItemsPerPage,
    totalItems,
    tableHead,
    tableBody,
    additionalItems,
    currentPage,
    setCurrentPage
}) => {
    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
                {title}
            </h1>
            <PaginatedList
                additionalItems = {additionalItems}
                itemsPerPage={itemsPerPage}
                setItemsPerPage={setItemsPerPage}
                isLoading={isLoading}
                totalItems={totalItems}
                tableHead={tableHead}
                tableBody={tableBody}
                currentPage={currentPage}
                setCurrentPage={setCurrentPage}
            />
        </div>

    );
};

export default PageOfItems;