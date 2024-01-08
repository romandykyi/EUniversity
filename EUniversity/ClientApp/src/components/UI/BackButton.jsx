import React from 'react';
import { Link } from 'react-router-dom';

const BackButton = ({ navigate }) => {
  return (
    <Link
      className='py-2 px-4 bg-theme text-xl font-bold text-white rounded-md cursor-pointer'
      to={`/${navigate}`}
    >
      back
    </Link>
  );
};

export default BackButton;
