import { React, useEffect, useState } from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import BackButton from '../UI/BackButton';

const AdminClassroom = () => {
  const [name, setName] = useState('');
  const [students, setStudents] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const location = useLocation();
  const isThemeDark = useAppSelector((state) => state.theme.isThemeDark);
  const regex = /\/classrooms\/(\d+)/;
  const classroomNumber = location.pathname.match(regex)[1];

  const fetchClassroom = async (page = 1, pageSize = 10) => {
    try {
      const response = await fetch(`/api/classrooms/${classroomNumber}`);
      if (response.ok) {
        const data = await response.json();
        setName(data.name);
        setIsLoading(false);
      } else {
        console.log('error');
      }
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchClassroom();
  }, []);

  return (
    <div className='students container max-w-[1100px] pt-10'>
      <div className='flex items-center gap-3 mb-14'>
        <BackButton navigate='classrooms' />
        <h1 className='students__title form__title mb-0'>
          Classroom #{classroomNumber} - {name}
        </h1>
      </div>
      {students.length ? (
        <>
          <div className='table-container'>
            <table
              className={`table table-hover ${isThemeDark ? 'table-dark' : ''}`}
            >
              <thead>
                <tr>
                  <th>First name</th>
                  <th>Last name</th>
                  <th>username</th>
                </tr>
              </thead>
              <tbody>
                {students.map((item) => (
                  <tr key={item.id}>
                    <td>{item.firstName}</td>
                    <td>{item.lastName}</td>
                    <td>{item.userName}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      ) : (
        <p className='text-gray-400 text-5xl text-center mt-[200px] fw-bold'>
          This classroom is empty
        </p>
      )}
    </div>
  );
};

export default AdminClassroom;
