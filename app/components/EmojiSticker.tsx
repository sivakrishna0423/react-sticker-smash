import { Image } from 'expo-image';
import { ImageSourcePropType, StyleSheet, View } from 'react-native';
type Props = {
    imageSize: number;
    stickerSource: ImageSourcePropType;
};
export default function EmojiSticker({ imageSize, stickerSource }: Props) {
  return (
   <View style={styles.stickerContainer}>
    <Image
      source={stickerSource} style={{ width: imageSize, height: imageSize, }} />
</View>
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