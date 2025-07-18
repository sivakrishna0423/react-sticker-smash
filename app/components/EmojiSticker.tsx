import { Image } from 'expo-image';
import { ImageSourcePropType, StyleSheet, View } from 'react-native';
import { Gesture, GestureDetector } from 'react-native-gesture-handler';
import Animated, { useAnimatedStyle, useSharedValue, withSpring } from 'react-native-reanimated';



type Props = {
    imageSize: number;
    stickerSource: ImageSourcePropType;
};
export default function EmojiSticker({ imageSize, stickerSource }: Props) 
{
  const scaleImage = useSharedValue(imageSize);
  const translateX = useSharedValue(0);
  const translateY = useSharedValue(0);
  const doubleTap = Gesture.Tap()
  .numberOfTaps(2)
  .onStart(() => {
    if(scaleImage.value !==imageSize * 2) {
  scaleImage.value = scaleImage.value * 2;
    } else {
      scaleImage.value = Math.round(scaleImage.value / 2);
    }
  });


const imageStyle = useAnimatedStyle(() => {
  return {
    width: withSpring(scaleImage.value),
    height: withSpring(scaleImage.value),
  };
});

const drag = Gesture.Pan().onChange((event) => {
  translateX.value += event.changeX;
  translateY.value += event.changeY;
});
const containerStyle = useAnimatedStyle(() => {
  return {
    transform: [
      { translateX: translateX.value,
      },
      { translateY: translateY.value 

      },

    ],
  };
});
  return (
    <GestureDetector gesture={drag}>
 
     <Animated.View style={styles.stickerContainer}>
 <GestureDetector gesture={doubleTap}>
  <View>
    <Animated.Image source={stickerSource} resizeMode={"contain"} style={{ width: imageSize, height: imageSize }} />
    <Image
      source={stickerSource} style={{ width: imageSize, height: imageSize, }} />
      </View>
      </GestureDetector>
      </Animated.View>
      </GestureDetector>

  );    
}
const styles = StyleSheet.create({
  stickerContainer: {
    position: 'absolute',
    top: 100, // Adjust as needed
    left: 100, // Adjust as needed
    zIndex: 10,
  },
});