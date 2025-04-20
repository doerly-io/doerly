import screenSizes from '../constants/screenSizes';
import useScreenSizeType from './useScreenSizeType';

const mobileSizes = [
  screenSizes.sizeTypes.xs,
  screenSizes.sizeTypes.sm,
] as any;

function useIsMobile() {
  const screenSizeType = useScreenSizeType();
  return mobileSizes.includes(screenSizeType);
}

export default useIsMobile;
