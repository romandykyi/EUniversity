import React, { useState } from 'react';
import Loader from "./UI/Loader/Loader";
import Button from "./UI/Button/Button"

const PaginatedList =
    ({
         items,
         itemsPerPage,
         isLoading,
         totalItems,
         currentPage,
         setCurrentPage
    }) => {

    const indexOfLastItem = currentPage * itemsPerPage;
    const indexOfFirstItem = indexOfLastItem - itemsPerPage;
    const currentItems = items.slice(indexOfFirstItem, indexOfLastItem);
    const totalPages = Math.ceil(totalItems / itemsPerPage);

    const paginate = (pageNumber) => {
        if (pageNumber >= 1 && pageNumber <= totalPages) {
            setCurrentPage(pageNumber);
        }
    };

    const handleNext = () => {
        if (currentPage < totalPages) {
            setCurrentPage(currentPage + 1);
        }
    };

    const handlePrev = () => {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1);
        }
    };

    const renderPageNumbers = () => {
        const pageNumbers = [];
        for (let i = 1; i <= totalPages; i++) {
            if (i === 1 || i === totalPages || (i >= currentPage - 1 && i <= currentPage + 1)) {
                pageNumbers.push(
                    <li
                        key={i}
                        onClick={() => paginate(i)}
                        className={i === currentPage ? 'pagination__chosen' : ''}
                    >
                        {i}
                    </li>
                );
            } else if (i === currentPage - 3 || i === currentPage + 3) {
                pageNumbers.push(<li key={i}>...</li>);
            }
        }
        return pageNumbers;
    };

    return (
        <>
            {
                isLoading
                ? <Loader/>
                : <table className="table students__table">
                        <thead>
                        <tr>
                            <th>Email</th>
                            <th>First name</th>
                            <th>Last name</th>
                            <th>Middle Name</th>
                        </tr>
                        </thead>
                        <tbody>
                        {currentItems.map((item) => (
                            <tr key={item.email}>
                                <td>{item.email}</td>
                                <td>{item.firstName}</td>
                                <td>{item.lastName}</td>
                                <td>{item.middleName}</td>
                            </tr>
                        ))}

                        </tbody>
                    </table>
            }

            <div className="pagination__panel">
                <Button onClick={handlePrev} disabled={currentPage === 1}>
                    Prev
                </Button>
                <ul className="pagination">
                    {renderPageNumbers()}
                </ul>
                <Button onClick={handleNext} disabled={currentPage === totalPages}>
                    Next
                </Button>
            </div>
        </>
    );
};

export default PaginatedList;
