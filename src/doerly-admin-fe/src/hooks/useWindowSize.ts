import { useEffect, useState } from 'react';

function getWindowSize(): [number, number] {
  const { innerWidth, innerHeight } = window;
  return [innerWidth, innerHeight];
}

function useWindowSize(): [number, number] {
  const [windowSize, setWindowSize] = useState<[number, number]>(
    getWindowSize()
  );

  useEffect(() => {
    function handleWindowResize() {
      setWindowSize(getWindowSize());
    }
    window.addEventListener('resize', handleWindowResize);
    return () => {
      window.removeEventListener('resize', handleWindowResize);
    };
  }, []);

  return windowSize;
}

export default useWindowSize;
