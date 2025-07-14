import * as imagepicker from 'expo-image-picker';
import { Link } from 'expo-router';
import { useState } from 'react';
import { ImageSourcePropType, StyleSheet, Text, View } from 'react-native';
import { GestureHandlerRootView } from 'react-native-gesture-handler';
import Button from '../components/Button';
import CircleButton from '../components/CircleButton';
import EmojiList from '../components/EmojiList';
import EmojiPicker from '../components/EmojiPicker';
import EmojiSticker from '../components/EmojiSticker';
import IconButton from '../components/IconButton';
import ImageViewer from '../components/ImageViewer';

const placeHolderImage = require("../assets/images/background-image.png");

export default function Index(){
  const[selectedImage, setSelectedImage] = useState<string | undefined>(undefined);
  const[showAppOptions, setShowAppOptions] = useState<boolean>(false);
  const[isModalVisible, setIsModalVisible] = useState<boolean>(false);
  const[pickedeEmoji, setPickedEmoji] = useState<ImageSourcePropType | undefined>(undefined);

  const pickImageAsync = async () => {
    let result = await imagepicker.launchImageLibraryAsync({
      mediaTypes:['images'],
      allowsEditing: true,
      quality: 1,
    });
    if(!result.canceled)
      {
      console.log(result);
      console.log("Selected Image URI:", result.assets[0].uri);
      setSelectedImage(result.assets[0].uri);
      setShowAppOptions(true);
    }
    else{
      alert("You did not select any image.");
    }
  };
const onReset=() => {
  setShowAppOptions(false);
}

const onAddSticker = () => {
setIsModalVisible(true);
}
  const onModalClose = () => {
    setIsModalVisible(false);
  };
const onSaveImageAsync = async () => {
}

  return(
    <GestureHandlerRootView style={styles.container}>
<View style={styles.container}>
  <Text style={styles.text}>Home Screen</Text>
  <Link href="/about" style={styles.button}>
        Go to About screen
      </Link>
  <View style = {styles.imageContainer}>
    <ImageViewer imgSource={placeHolderImage} selectedImage = {selectedImage}/>
    {pickedeEmoji && <EmojiSticker imageSize={40} stickerSource={pickedeEmoji}/>}
  </View>
{showAppOptions ? (
  <View style={styles.optionsContainer}>
    <View style={styles.optionsRow}>
      <IconButton icon="refresh" label="Reset" onPress={onReset} />
      <CircleButton onPress= {onAddSticker} />
      <IconButton icon ="save-alt" label="Save" onPress={onSaveImageAsync} />
      </View>
  </View>
) : (
    <View style={styles.footerContainer}>
     <Button theme="primary" label="Choose  photo" onPress={pickImageAsync} />
        <Button theme="secondary" label="Use this photo" onPress={()=> setShowAppOptions(true)} />
     </View>
  )}
<EmojiPicker visible={isModalVisible} onClose={onModalClose}>
  <EmojiList onSelect={setPickedEmoji} onCloseModal={onModalClose} />
  </EmojiPicker>
</View>
</GestureHandlerRootView>
);
}

const styles=StyleSheet.create({
  container:{
    flex:1,
    backgroundColor:'#25292e',
    alignItems:'center',
    justifyContent:'center',
  },
  text:{
    color: '#fff',
  },
  button: {
    fontSize: 20,
    textDecorationLine: 'underline',
    color: '#fff',
  },
  imageContainer: {
    flex: 1
  },
  image:{
    width:320,
    height:440,
    borderRadius:18,
  },
   footerContainer: {
    flex: 1 / 3,
    alignItems: 'center',
  },
  footerContainer1: {
  position: 'absolute',
  bottom: 40,
  alignItems: 'center',
  width: '100%',
},
    optionsContainer: {
    position: 'absolute',
    bottom: 80,
  },
  optionsRow: {
    alignItems: 'center',
    flexDirection: 'row',
  },
});
