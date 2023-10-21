import React, {useEffect, useState} from 'react';
import Loader from "./UI/Loader/Loader";
import Button from "./UI/Button/Button"

const PaginatedList =
    ({
         itemsPerPage,
         isLoading,
         totalItems,
         setItemsPerPage,
         tableBody,
         tableHead,
         fetchItems
    }) => {

    const [totalPages, setTotalPages] = useState(Math.ceil(totalItems / itemsPerPage));
    const [currentPage, setCurrentPage] = useState(1);
    const [windowSize, setWindowSize] = useState({
        width: window.innerWidth,
        height: window.innerHeight,
    });

    useEffect(() => {
        fetchItems(1,10);
    }, []);

    useEffect(() => {
        fetchItems(currentPage, itemsPerPage);
    }, [currentPage, itemsPerPage]);

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
            if (newTotalPages > 1) setCurrentPage(newTotalPages);
            else setCurrentPage(1);
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
        console.log(totalPages);
        const pageNumbers = [];
        if (totalPages <= 7) {
            addPageButtons(1, totalPages, pageNumbers);
        }
        else if (currentPage < 5) {
            addPageButtons(1, 5, pageNumbers);
            addThreeDotsButtons('right-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages, totalPages, pageNumbers);
        }
        else if (currentPage > totalPages - 4) {
            addPageButtons(1, 1, pageNumbers);
            addThreeDotsButtons('left-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages - 4, totalPages, pageNumbers);
        }
        else {
            addPageButtons(1, 1, pageNumbers);
            addThreeDotsButtons('left-three-dots-pagination', pageNumbers);
            const start = currentPage - 1;
            const end = currentPage + 1;
            addPageButtons(start, end, pageNumbers);
            addThreeDotsButtons('right-three-dots-pagination', pageNumbers);
            addPageButtons(totalPages, totalPages,pageNumbers);
        }
        return pageNumbers;
    }

    const addThreeDotsButtons = (key, pageNumbers) => {
        pageNumbers.push(<button disabled className="pagination__panelbutton" key={key}>...</button>);
    };

    const addPageButtons = (start, end, pageNumbers) => {
        for (let i = start; i <= end; i++) {
            const clName = i === currentPage ? 'pagination__chosen' : '';
            pageNumbers.push(<button key={i} className={`${clName} pagination__panelbutton`} onClick={() => paginate(i)}>{i}</button>);
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
                :  <div className="table-container">
                    <table className="table students__table">
                        <thead>
                            {tableHead}
                        </thead>
                       <tbody>
                            {tableBody}
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
                                onChange={e => setCurrentPage(e.target.value)}>
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
                    <option defaultValue disabled>Items per page:</option>
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
