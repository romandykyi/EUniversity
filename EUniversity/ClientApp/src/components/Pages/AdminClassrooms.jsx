import React from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DeleteModal from '../DeleteModal';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button';
import PageForm from '../PageForm';
import EditFormModal from '../EditFormModal';

const AdminClassrooms = () => {
  const [classrooms, setClassrooms] = useState([]);
  const [pageSize, setPageSize] = useState(10);
  const [page, setPage] = useState(1);
  const [isDeleteVisible, setIsDeleteVisible] = useState(false);
  const [inputValue, setInputValue] = useState('');
  const [sortingMethod, setSortingMethod] = useState(0);
  const [isEditable, setIsEditable] = useState(false);
  const [editableItem, setEditableItem] = useState(null);
  const [deletedClassroom, setDeletedClassroom] = useState({
    id: '',
    name: '',
  });

  const isAdmin = useAppSelector((state) => state.isAdmin.isAdmin);
  const navigate = useNavigate();

  const deleteClassroom = async (classroomId) => {
    try {
      const response = await fetch(`/api/classrooms/${classroomId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
      });
      if (response.ok) {
        setIsDeleteVisible();
      } else {
        console.log('error');
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <PageForm
      setItems={setClassrooms}
      additionalComponents={
        <>
          <DeleteModal
            isVisible={isDeleteVisible}
            setIsVisible={setIsDeleteVisible}
            itemType='classroom'
            deleteFunction={deleteClassroom}
            deletedItem={deletedClassroom}
          />
          {editableItem ? (
            <EditFormModal
              item={editableItem}
              isEditable={isEditable}
              setIsEditable={setIsEditable}
              itemType='classroom'
              responseTitle='classrooms'
            />
          ) : (
            ''
          )}
        </>
      }
      registerTitle='classrooms'
      tableBody={classrooms.map((item) => (
        <tr
          onClick={() => {
            navigate(`${item.id}`);
          }}
          key={item.id}
          className='cursor-pointer'
        >
          <td>{item.name}</td>
          {isAdmin ? (
            <>
              <td>
                <Button
                  onClick={(e) => {
                    e.stopPropagation();
                    setIsDeleteVisible(true);
                    setDeletedClassroom({ id: item.id, name: item.name });
                  }}
                >
                  Delete Classroom
                </Button>
              </td>
              <td>
                <Button
                  onClick={(e) => {
                    e.stopPropagation();
                    setIsEditable(true);
                    setEditableItem(item);
                  }}
                >
                  Edit Classroom
                </Button>
              </td>
            </>
          ) : (
            ''
          )}
        </tr>
      ))}
      tableHead={
        <tr>
          <th>Name</th>
          {isAdmin ? (
            <>
              <th>Delete</th>
              <th>Edit</th>
            </>
          ) : (
            ''
          )}
        </tr>
      }
      searchLink={`/api/classrooms?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`}
      fetchLink={`/api/classrooms?Page=${page}&PageSize=${pageSize}&sortingMode=${sortingMethod}`}
      currentPage={page}
      setCurrentPage={setPage}
      itemsPerPage={pageSize}
      setItemsPerPage={setPageSize}
      inputValue={inputValue}
      setInputValue={setInputValue}
      isDeleteVisible={isDeleteVisible}
      setSortingMethod={setSortingMethod}
      sortingMethod={sortingMethod}
      isEditVisible={isEditable}
    />
  );
};

export default AdminClassrooms;
