import React, {useEffect, useState} from 'react';
import Loader from "./UI/Loader/Loader";
import Button from "./UI/Button/Button"

const PaginatedList =
    ({
         items,
         itemsPerPage,
         isLoading,
         totalItems,
         currentPage,
         setCurrentPage,
         setItemsPerPage
    }) => {

    const [totalPages, setTotalPages] = useState(Math.ceil(totalItems / itemsPerPage));

    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });
        useEffect(() => {
            const handleResize = () => {
                setWindowSize({
                    width: window.innerWidth,
                    height: window.innerHeight,
                });
            };

            window.addEventListener('resize', handleResize);

            return () => {
                window.removeEventListener('resize', handleResize);
            };
        }, []);
    useEffect(() => {
        const newTotalPages = Math.ceil(totalItems / itemsPerPage);
        setTotalPages(newTotalPages);

        if (currentPage > newTotalPages) {
            setCurrentPage(newTotalPages);
           }
        }, [itemsPerPage, totalItems, currentPage]);

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
        const maxPagesVisible = 7;
        const maxBoundaryPagesVisible = 5;
        if (totalPages <= maxPagesVisible) {
            addPageButtons(1, totalPages, pageNumbers);
        }
        else if (currentPage < maxBoundaryPagesVisible) {
            addPageButtons(1, maxBoundaryPagesVisible, pageNumbers);
            addThreeDotsButtons('right-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages - 1 + 1, totalPages, pageNumbers);
        }
        else if (currentPage > totalPages - maxBoundaryPagesVisible + 1) {
            addPageButtons(1, 1, pageNumbers);
            addThreeDotsButtons('left-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages - maxBoundaryPagesVisible + 1, totalPages, pageNumbers);
        }
        else {
            addPageButtons(1, 1, pageNumbers);
            addThreeDotsButtons('left-three-dots-pagination', pageNumbers);
            const offset = 1;
            const start = currentPage - Math.floor(offset);
            const end = currentPage + Math.ceil(offset);
            addPageButtons(start, end, pageNumbers);
            addThreeDotsButtons('right-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages - 1 + 1, totalPages,pageNumbers);
        }
        return pageNumbers;
    }

    const addThreeDotsButtons = (key, pageNumbers) => {
        pageNumbers.push(<li key={key}>...</li>);
    };

    const addPageButtons = (start, end, pageNumbers) => {
        for (let i = start; i <= end; i++) {
            const clName = i === currentPage ? 'pagination__chosen' : '';
            pageNumbers.push(<li key={i} className={clName} onClick={() => paginate(i)}>{i}</li>);
        }
    };

    const renderPageNumbersForSelect = () => {
        const pageNumbers = [];
        for (let i = 1; i <= totalPages; i++) {
            pageNumbers.push(
                <option key={i} value={i}>
                    {i}
                </option>
            );
        }
        return pageNumbers;
    };

    return (
        <>
            {
                isLoading
                ? <Loader/>
                :  <div className="table-container" style={{ overflowX: 'auto' }}>
                    <table className="table students__table">
                        <thead>
                        <tr>
                            <th>Email</th>
                            <th>First name</th>
                            <th>Last name</th>
                            <th>Middle Name</th>
                        </tr>
                        </thead>
                        <tbody>
                        {items.map((item) => (
                            <tr key={item.email}>
                                <td>{item.email}</td>
                                <td>{item.firstName}</td>
                                <td>{item.lastName}</td>
                                <td>{item.middleName}</td>
                            </tr>
                        ))}

                        </tbody>
                    </table>
                    </div>
            }
            <div className="pagination__panel">
                <div className="pagination__main">
                    <Button onClick={handlePrev} disabled={currentPage === 1}>
                        Prev
                    </Button>
                    {
                        windowSize.width > 615
                            ?  <ul className="pagination">
                                {renderPageNumbers()}
                            </ul>
                            : <select
                                style={{margin:"0 10px"}}
                                className="form-select"
                                value={currentPage}
                                onChange={(e) => setCurrentPage(e.target.value)}>
                                {renderPageNumbersForSelect().map((pageNumber, index) => (
                                    <option key={index} value={pageNumber.key}>
                                        {pageNumber.key}
                                    </option>
                                ))}
                            </select>
                    }
                    <Button onClick={handleNext} disabled={currentPage === totalPages}>
                        Next
                    </Button>
                </div>
                <select
                    className="form-select"
                    id="floatingSelect"
                    value = {itemsPerPage}
                    onChange={e => {
                        setItemsPerPage(e.target.value);
                        if (currentPage > totalPages) {
                            setCurrentPage(totalPages);
                    }
                }}>
                    <option selected disabled>Items per page:</option>
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                    <option value={20}>20</option>
                    <option value={100}>100</option>
                </select>
            </div>
        </>
    );
};

export default PaginatedList;
