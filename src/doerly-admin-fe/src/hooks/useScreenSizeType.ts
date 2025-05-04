import { useEffect, useState } from 'react';
import screenSizes from '../constants/screenSizes';

// Descending ordering (from 999 to 0)
const orderedSizeTypes = Object.entries(screenSizes.sizesToTypes)
  .sort(([, value1], [, value2]) => value2 - value1)
  .map(([type]) => type);

const getScreenSizeType = () => {
  const screenWidth = window.innerWidth;
  return orderedSizeTypes.find(
    (type) => screenSizes.sizesToTypes[type] <= screenWidth
  );
};

function useScreenSizeType() {
  const [screenSizeType, setScreenSizeType] = useState(getScreenSizeType());
  useEffect(() => {
    function onWindowResize() {
      setScreenSizeType(getScreenSizeType());
    }
    window.addEventListener('resize', onWindowResize);
    return () => window.removeEventListener('resize', onWindowResize);
  }, []);

  return screenSizeType;
}

export default useScreenSizeType;
