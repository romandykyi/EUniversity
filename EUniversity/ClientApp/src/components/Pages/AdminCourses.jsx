import React from 'react';
import PageOfItems from '../PageOfItems';
import { useState } from 'react';

const AdminGroup = () => {

    const [courses, setCourses] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);

    const fetchCourses = async(page = 1, pageSize = 10) => {

        try {
            const response = await fetch(`/api/courses?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setCourses(data.items);
                setItemsPerPage(data.pageSize);
                setTotalItems(data.totalItemsCount);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }

    };

    return (
        <>
            <PageOfItems
                title = 'All Courses'
                fetchFunction = {fetchCourses}
                isLoading = {isLoading}
                itemsPerPage = {itemsPerPage}
                setItemsPerPage = {setItemsPerPage}
                totalItems = {totalItems}
                tableHead = {(
                    <tr>
                        <th>Name of the course</th>
                    </tr>
                )}
                tableBody = {(
                    courses.map((item) => (
                        <tr 
                            key={item.id}
                        >
                            <td>{item.name}</td>
                        </tr>
                    ))
                )}
            />   
        </>
    );
};

export default AdminGroup;