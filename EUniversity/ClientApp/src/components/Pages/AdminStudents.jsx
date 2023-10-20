    import React, {useEffect, useState} from 'react';
    import PaginatedList from "../PaginatedList";

    const AdminStudents = () => {

        const [students, setStudents] = useState([]);
        const [isLoading, setIsLoading] = useState(true);
        const [itemsPerPage, setItemsPerPage] = useState(0);
        const [totalItems, setTotalItems] = useState(0);
        const [currentPage, setCurrentPage] = useState(1);

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

        useEffect(() => {
            fetchUsers();
        }, []);

        useEffect(() => {
            setIsLoading(true);
            fetchUsers(currentPage, itemsPerPage);
        }, [currentPage, itemsPerPage]);

        return (
            <div className="students">
                <h1 className="students__title form__title">
                    All students
                </h1>
                <PaginatedList
                    itemsPerPage={itemsPerPage}
                    setItemsPerPage={setItemsPerPage}
                    items={students}
                    isLoading={isLoading}
                    totalItems={totalItems}
                    currentPage={currentPage}
                    setCurrentPage={setCurrentPage}
                />
            </div>

        );
    };

    export default AdminStudents;