    import React, {useEffect, useState} from 'react';
    import PaginatedList from "../PaginatedList";

    const AdminStudents = () => {

        const [students, setStudents] = useState([]);
        const [isLoading, setIsLoading] = useState(true);
        const [itemsPerPage, setItemsPerPage] = useState(10);
        const [totalItems, setTotalItems] = useState(0);

        const fetchUsers = async(page = 1, pageSize = 10) => {
            try {
                const response = await fetch(`/api/users/students?Page=${page}&PageSize=${pageSize}`);
                if (response.ok) {
                    const data = await response.json();

                    setStudents(data.items);
                    setItemsPerPage(data.pageSize);
                    setTotalItems(data.totalItemsCount);
                    setIsLoading(false);

                } else {
                    console.log('error');
                }
            } catch(error) {
                console.log(error);
            }

        }


        return (
            <div className="students">
                <h1 className="students__title form__title">
                    All students
                </h1>
                <PaginatedList
                    itemsPerPage={itemsPerPage}
                    setItemsPerPage={setItemsPerPage}
                    isLoading={isLoading}
                    totalItems={totalItems}
                    fetchItems={fetchUsers}
                    tableHead={(
                        <tr>
                            <th>Email</th>
                            <th>First name</th>
                            <th>Last name</th>
                            <th>Middle Name</th>
                        </tr>
                    )}
                    tableBody={(
                        students.map((item) => (
                            <tr key={item.id}>
                                <td>{item.email}</td>
                                <td>{item.firstName}</td>
                                <td>{item.lastName}</td>
                                <td>{item.middleName}</td>
                            </tr>
                        ))
                    )}
                />
            </div>

        );
    };

    export default AdminStudents;