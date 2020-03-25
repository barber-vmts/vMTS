<?php

if ( isset($_POST["imageName"]) && !empty($_POST["imageName"]) ) {    

define('UPLOAD_DIR', 'E:/web/learntor/images/sponsorlogo/');

    $dataName = $_POST["imageName"];  

    $file = UPLOAD_DIR . $dataName . '.jpg';

    // delete the file from the upload directory
	   if(file_exists($file)){
			$success = unlink($file);
	   }else{
			$success = "False";
	   }

    // return the message (success)
    echo $success ? "True" : "False";
    }else{
    echo "Fail";
    }
 ?> 