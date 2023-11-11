    import React, {useEffect, useState} from 'react';
    import PaginatedList from "../PaginatedList";

    const AdminUsers = () => {

        const [students, setStudents] = useState([]);
        const [isLoading, setIsLoading] = useState(true);
        const [itemsPerPage, setItemsPerPage] = useState(10);
        const [totalItems, setTotalItems] = useState(0);
        const [usersType, setUsersType] = useState('students');

        const fetchUsers = async(page = 1, pageSize = 10) => {
            try {
                const response = await fetch(`/api/users/${usersType}?Page=${page}&PageSize=${pageSize}`);
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
        
        const changeUsersType = async e => {
            setUsersType(e.target.value);
            await fetchUsers();
        }


        return (
            <div className="students container max-w-[1100px] pt-10">
                <h1 className="students__title form__title">
                    All {usersType}
                </h1>
                <div className="students__select_wrapper">
                    <select className="form-select students__select" onChange={changeUsersType}>
                        <option value="students">Students</option>
                        <option value="teachers">Teachers</option>
                    </select>
                </div>
                <PaginatedList
                    itemsPerPage={itemsPerPage}
                    setItemsPerPage={setItemsPerPage}
                    isLoading={isLoading}
                    totalItems={totalItems}
                    fetchItems={fetchUsers}
                    usersType={usersType}
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

    export default AdminUsers;