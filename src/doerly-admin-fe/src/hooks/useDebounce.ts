/* eslint-disable import/prefer-default-export */
import { useEffect, useState } from 'react';

const useDebounce = ({ value, delay = 500 }: IProps) => {
  const [debouncedValue, setDebouncedValue] = useState(value);

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedValue(value), delay);

    return () => {
      clearTimeout(timer);
    };
  }, [value, delay]);

  return debouncedValue;
};

interface IProps {
  delay?: number;
  value: any;
}

export default useDebounce;
