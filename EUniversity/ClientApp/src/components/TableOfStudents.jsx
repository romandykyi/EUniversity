import React from 'react';
import {PaginatedList} from "react-paginated-list";
import Loader from "./UI/Loader/Loader";


const TableOfStudents = ({itemsPerPage, items, isLoading}) => {
    return (
        <PaginatedList
            list={items}
            itemsPerPage={itemsPerPage}
            isLoading={isLoading}
            loadingItem={() => <Loader/>}
            nextText="Next"
            prevText="Prev"
            renderList={(list) => (
                <table className="table students__table">
                    <tr>
                        <th>Email</th>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Middle Name</th>
                    </tr>
                    {list.map((item) => {
                        return (
                            <tr key={item.email}>
                                <td>{item.email}</td>
                                <td>{item.firstName}</td>
                                <td>{item.lastName}</td>
                                <td>{item.middleName}</td>
                            </tr>
                        );
                    })}
                </table>
            )}
        />
    );
};

export default TableOfStudents;