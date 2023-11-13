import React from 'react';
import PageOfItems from '../PageOfItems';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AdminClassrooms = () => {

    const [classrooms, setClassrooms] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const navigate = useNavigate();

    const fetchClassrooms = async(page = 1, pageSize = 10) => {

        try {
            const response = await fetch(`/api/classrooms?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setClassrooms(data.items);
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
                title = 'All Classrooms'
                fetchFunction = {fetchClassrooms}
                isLoading = {isLoading}
                itemsPerPage = {itemsPerPage}
                setItemsPerPage = {setItemsPerPage}
                totalItems = {totalItems}
                tableHead = {(
                    <tr>
                        <th>Name</th>
                    </tr>
                )}
                tableBody = {(
                    classrooms.map((item) => (
                        <tr 
                            onClick={() => {
                                navigate(`${item.id}`);
                            }} 
                            key={item.id} className="cursor-pointer"
                        >
                            <td>{item.name}</td>
                        </tr>
                    ))
                )}
            />   
        </>
    );
};

export default AdminClassrooms;