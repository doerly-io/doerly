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

function useIsSmallMobile() {
  const screenSizeType = useScreenSizeType();
  return screenSizeType === screenSizes.sizeTypes.xs;
}

function useIsDesktop() {
  const screenSizeType = useScreenSizeType();
  return screenSizeType === screenSizes.sizeTypes.xl;
}

export { useIsMobile, useIsSmallMobile, useIsDesktop };

export default useIsMobile;
