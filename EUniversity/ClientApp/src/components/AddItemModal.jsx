import { useEffect, useState } from 'react';
import Table from './UI/Table/Table';
import Button from './UI/Button';
import SearchSelect from './UI/SearchSelect';
import SearchSelectForClasses from './UI/SearchSelectForClasses';

const AddClassroomModal = ({
  isVisible,
  setIsVisible,
  title,
  responseTitle,
  fetchItems,
}) => {
  const [errors, setErrors] = useState([]);
  const [items, setItems] = useState([]);
  const [itemParams, setItemParams] = useState({});
  const [tableHead, setTableHead] = useState();
  const [tableBody, setTableBody] = useState();

  useEffect(() => {
    switch (responseTitle) {
      case 'users':
        setTableHead(
          <tr>
            <th>Email</th>
            <th>First Name</th>
            <th>Last Name</th>
            <th>Middle Name</th>
            <th>Role</th>
            <th>Delete</th>
          </tr>,
        );
        setTableBody(
          items.map((row) => (
            <tr key={row.id}>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='email'
                  value={row.email}
                  onChange={(e) =>
                    handleInputChange(row.id, 'email', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='first name'
                  value={row.firstName}
                  onChange={(e) =>
                    handleInputChange(row.id, 'firstName', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  value={row.lastName}
                  placeholder='last name'
                  onChange={(e) =>
                    handleInputChange(row.id, 'lastName', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='middle name'
                  value={row.middleName}
                  onChange={(e) =>
                    handleInputChange(row.id, 'middleName', e.target.value)
                  }
                />
              </td>
              <td>
                <select
                  className='form-select w-40 bg-background text-text'
                  onChange={(e) =>
                    handleInputChange(row.id, 'role', e.target.value)
                  }
                >
                  <option value='student'>student</option>
                  <option value='teacher'>teacher</option>
                </select>
              </td>
              <td>
                <Button
                  onClick={(e) => {
                    e.preventDefault();
                    deleteItem(row.id);
                  }}
                >
                  Delete
                </Button>
              </td>
            </tr>
          )),
        );
        setItemParams({
          email: '',
          firstName: '',
          lastName: '',
          middleName: '',
        });
        break;
      case 'groups':
        setTableHead(
          <tr>
            <th>Name</th>
            <th>Teacher</th>
            <th>Course</th>
          </tr>,
        );
        setTableBody(
          items.map((row) => (
            <tr key={row.id}>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='name'
                  value={row.name}
                  onChange={(e) =>
                    handleInputChange(row.id, 'name', e.target.value)
                  }
                />
              </td>
              <td>
                <SearchSelect
                  handleInputChange={handleInputChange}
                  itemId={row.id}
                  link='/api/users/teachers?Page=1&PageSize=20&FullName='
                  title='teachers'
                />
              </td>
              <td>
                <SearchSelect
                  handleInputChange={handleInputChange}
                  itemId={row.id}
                  link='/api/courses?name='
                  title='courses'
                />
              </td>
            </tr>
          )),
        );
        setItemParams({
          name: '',
          course: 0,
          teacher: '',
        });
        break;
      case 'semesters':
        setTableHead(
          <tr>
            <th>Name</th>
            <th>From</th>
            <th>To</th>
          </tr>,
        );
        setTableBody(
          items.map((row) => (
            <tr key={row.id}>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='name'
                  value={row.name}
                  onChange={(e) =>
                    handleInputChange(row.id, 'name', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='date'
                  placeholder='from'
                  value={row.dateFrom}
                  onChange={(e) =>
                    handleInputChange(row.id, 'dateFrom', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='date'
                  placeholder='to'
                  value={row.dateTo}
                  onChange={(e) =>
                    handleInputChange(row.id, 'dateTo', e.target.value)
                  }
                />
              </td>
            </tr>
          )),
        );
        setItemParams({
          name: '',
          course: 0,
          teacher: '',
        });
        break;
      case 'classes':
        setTableHead(
          <tr>
            <th>Class type</th>
            <th>Classroom</th>
            <th>Group</th>
            <th>Start date</th>
            <th>Start time</th>
            <th>Duration</th>
            <th>Repeats</th>
            <th>Repeat delay (in days)</th>
          </tr>,
        );
        setTableBody(
          items.map((row) => (
            <tr key={row.id}>
              <td>
                <SearchSelectForClasses
                  handleInputChange={handleInputChange}
                  itemId={row.id}
                  link='/api/classTypes?name='
                  title='classType'
                />
              </td>
              <td>
                <SearchSelectForClasses
                  handleInputChange={handleInputChange}
                  itemId={row.id}
                  link='/api/classrooms?name='
                  title='classroom'
                />
              </td>
              <td>
                <SearchSelectForClasses
                  handleInputChange={handleInputChange}
                  itemId={row.id}
                  link='/api/groups?name='
                  title='group'
                />
              </td>
              <td>
                <input
                  className='bg-background text-text'
                  type='date'
                  placeholder='from'
                  value={row.startDate}
                  onChange={(e) =>
                    handleInputChange(row.id, 'startDate', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  type='time'
                  onChange={(e) =>
                    handleInputChange(row.id, 'startTime', e.target.value)
                  }
                />
              </td>
              <td>
                <input
                  className='bg-background'
                  type='text'
                  pattern='[0-2][0-9]:[0-5][0-9]'
                  onChange={(e) =>
                    handleInputChange(row.id, 'duration', e.target.value)
                  }
                  placeholder='example: 01:45'
                />
              </td>
              <td>
                <input
                  type='number'
                  onChange={(e) =>
                    handleInputChange(row.id, 'repeats', e.target.value)
                  }
                  placeholder='provide a number'
                />
              </td>

              <td>
                <input
                  type='number'
                  onChange={(e) =>
                    handleInputChange(
                      row.id,
                      'repeatsDelayDays',
                      e.target.value,
                    )
                  }
                  placeholder='provide a number'
                />
              </td>
            </tr>
          )),
        );
        setItemParams({
          name: '',
        });
        break;
      default:
        setTableHead(
          <tr>
            <th>Name</th>
          </tr>,
        );
        setTableBody(
          items.map((row) => (
            <tr key={row.id}>
              <td>
                <input
                  className='bg-background text-text'
                  type='text'
                  placeholder='name'
                  value={row.name}
                  onChange={(e) =>
                    handleInputChange(row.id, 'name', e.target.value)
                  }
                />
              </td>
            </tr>
          )),
        );
        setItemParams({
          name: '',
        });
        break;
    }
  }, [items]);

  const getCurrentDateTime = () => {
    const currentDate = new Date();

    const year = currentDate.getFullYear();
    const month = String(currentDate.getMonth() + 1).padStart(2, '0');
    const day = String(currentDate.getDate()).padStart(2, '0');

    const hours = String(currentDate.getHours()).padStart(2, '0');
    const minutes = String(currentDate.getMinutes()).padStart(2, '0');
    const seconds = String(currentDate.getSeconds()).padStart(2, '0');

    const milliseconds = String(currentDate.getMilliseconds()).padStart(7, '0');

    const timezoneOffset = -currentDate.getTimezoneOffset();
    const timezoneOffsetHours = Math.floor(Math.abs(timezoneOffset) / 60)
      .toString()
      .padStart(2, '0');
    const timezoneOffsetMinutes = (Math.abs(timezoneOffset) % 60)
      .toString()
      .padStart(2, '0');
    const timezoneOffsetSign = timezoneOffset >= 0 ? '+' : '-';

    const formattedDate = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}.${milliseconds}${timezoneOffsetSign}${timezoneOffsetHours}:${timezoneOffsetMinutes}`;

    return formattedDate;
  };

  const convertToISOString = (input) => {
    const [time, date] = input.split(' ');
    const [hours, minutes] = time.split(':');
    const inputDate = new Date(`${date}T${hours}:${minutes}:00Z`);
    const year = inputDate.getUTCFullYear();
    const month = String(inputDate.getUTCMonth() + 1).padStart(2, '0');
    const day = String(inputDate.getUTCDate()).padStart(2, '0');
    const hoursUTC = String(inputDate.getUTCHours()).padStart(2, '0');
    const minutesUTC = String(inputDate.getUTCMinutes()).padStart(2, '0');
    const secondsUTC = String(inputDate.getUTCSeconds()).padStart(2, '0');
    const milliseconds = String(inputDate.getUTCMilliseconds()).padStart(
      3,
      '0',
    );
    return `${year}-${month}-${day}T${hoursUTC}:${minutesUTC}:${secondsUTC}.${milliseconds}Z`;
  };

  const timeToTicks = (timeString) => {
    const [hours, minutes, seconds] = timeString.split(':');
    const totalSeconds =
      parseInt(hours, 10) * 3600 +
      parseInt(minutes, 10) * 60 +
      parseInt(seconds, 10);
    return totalSeconds * 10000000;
  };

  const getResponseForUsers = async (usersType, usersList) => {
    try {
      const response = await fetch(`/api/users/${usersType}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: `{"users": ${JSON.stringify(usersList)}}`,
      });

      if (response.ok) {
        setIsVisible(false);
        document.body.style.overflow = 'auto';
      } else {
        const responseBody = await response.json();
        const errors = responseBody.errors;
        let errorsArray = [];
        for (let key in errors) {
          const value = errors[key];
          for (let er of value) {
            errorsArray.push(er);
          }
        }
        setErrors(errorsArray);
      }
    } catch (error) {
      setErrors([`An error occurred while adding new ${title}.`]);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    let postItems = [];

    if (responseTitle === 'users') {
      const students = [
        ...items.filter((user) => user.role === 'student'),
        ...items.filter((user) => !user.role),
      ];
      const teachers = items.filter((user) => user.role === 'teacher');

      const postStudents = students.map((user) => ({
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        middleName: user.middleName,
      }));
      const postTeachers = teachers.map((user) => ({
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        middleName: user.middleName,
      }));
      if (students.length) await getResponseForUsers('students', postStudents);
      if (teachers.length) await getResponseForUsers('teachers', postTeachers);
      setItems([]);
      await fetchItems();
    } else if (responseTitle === 'groups') {
      postItems = items.map((item) => ({
        name: item.name,
        courseId: item.course,
        teacherId: item.teacher,
      }));
    } else if (responseTitle === 'semesters') {
      postItems = items.map((item) => ({
        name: item.name,
        creationDate: getCurrentDateTime(),
        updateDate: getCurrentDateTime(),
        dateFrom: item.dateFrom,
        dateTo: item.dateTo,
      }));
    } else if (responseTitle === 'classes') {
      postItems = items.map((item) => ({
        classTypeId: item.classType,
        classroomId: item.classroom,
        groupId: item.group,
        startDate: convertToISOString(`${item.startTime} ${item.startDate}`),
        duration: `${item.duration}:00`,
        repeats: parseInt(item.repeats),
        repeatsDelayDays: parseInt(item.repeatsDelayDays),
      }));
    } else {
      postItems = items.map((item) => ({
        name: item.name,
      }));
    }

    if (responseTitle !== 'users') {
      for (let postItem of postItems) {
        console.log(postItem);
        try {
          const response = await fetch(`/api/${responseTitle}`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: `${JSON.stringify(postItem)}`,
          });
          if (response.ok) {
            setIsVisible(false);
            document.body.style.overflow = 'auto';
            setItems([]);
            setErrors([]);
            await fetchItems();
          } else {
            const responseBody = await response.json();
            const errors = responseBody.errors;
            let errorsArray = [];
            for (let key in errors) {
              const value = errors[key];
              for (let er of value) {
                errorsArray.push(er);
              }
            }
            setErrors(errorsArray);
          }
        } catch (error) {
          setErrors([`An error occurred while adding new ${title}.`]);
        }
      }
    }
  };

  const handleClickOnBg = () => {
    setIsVisible(false);
    setErrors([]);
    document.body.style.overflow = 'auto';
  };

  const handleInputChange = (id, field, value) => {
    const newData = items.map((row) =>
      row.id === id ? { ...row, [field]: value } : row,
    );
    setItems(newData);
  };

  const deleteItem = (id) => {
    const newData = items.filter((item) => item.id !== id);
    setItems(newData);
  };

  return (
    <div
      onClick={handleClickOnBg}
      className={`${
        isVisible ? 'fixed' : 'hidden'
      } top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-40 flex items-center justify-center px-4 overflow-auto`}
    >
      <div
        className=' container max-w-[1100px] pt-10 bg-background p-10 rounded-lg z-50'
        onClick={(e) => e.stopPropagation()}
      >
        <div className='flex justify-between items-center mb-20'>
          <h1 className='newUser__title form__title m-0'>
            Register new {title}
          </h1>
          <Button addStyles='bg-danger' onClick={handleClickOnBg}>
            Close
          </Button>
        </div>
        <form onSubmit={handleSubmit} className='newUser form__form'>
          <Table
            items={items}
            setItems={setItems}
            title={title}
            itemParams={itemParams}
            tableHead={tableHead}
            tableBody={tableBody}
            isAddMoreDisable={responseTitle === 'users' ? true : false}
          />
          <div className='newUser__error form__error'>
            {errors.map((error) => (
              <>
                <p className='text-danger'>{error}</p>
              </>
            ))}
          </div>
          <Button type='submit'>Register new {title}</Button>
        </form>
      </div>
    </div>
  );
};

export default AddClassroomModal;
