import React from 'react';
import { Spinner } from 'react-bootstrap';

const LoadingSpinner = ({ message }) => {
  return (
    <div className="d-flex justify-content-center align-items-center mt-4">
      <Spinner animation="border" role="status" className="mr-2">
        <span className="sr-only">Loading...</span>
      </Spinner>
      <span>{message}</span>
    </div>
  );
};

export default LoadingSpinner;
