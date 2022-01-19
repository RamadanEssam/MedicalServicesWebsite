 let pos = 0;

 let slider = document.getElementById('myslider');

 document.getElementById('rightBtn').onclick = () => {
     if (pos === -300) {
         slider.style.left = 0;
         pos = 0;
     } else {
         pos -= 100;
         slider.style.left = pos + '%';
     }

 }

 document.getElementById('leftBtn').onclick = () => {
     if (pos === 0) {
         slider.style.left = '-300%';
         pos = -300;
     } else {
         pos += 100;
         slider.style.left = pos + '%';
     }
 }