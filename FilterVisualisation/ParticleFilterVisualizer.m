%First line filter : x z yaw 
%Rest of the lines : x z weight

clear;
markerconfigfile = '../IRescue/UserLocalisation/Maps/MarkerMap01.xml' ;
fovdistance = 0.5;
xDoc = xmlread(markerconfigfile);
xRoot = xDoc.getDocumentElement;
allListitems = xDoc.getElementsByTagName('marker');
minx = str2num(allListitems.item(0).getElementsByTagName('x').item(0).getFirstChild.getData);
maxx = minx;
minz = str2num(allListitems.item(0).getElementsByTagName('z').item(0).getFirstChild.getData);
maxz = minz;
for k = 1:allListitems.getLength-1
   thisListitem = allListitems.item(k);
   
   x = str2num(thisListitem.getElementsByTagName('x').item(0).getFirstChild.getData);
   if x < minx
       minx = x;
   elseif x > maxx
       maxx = x;
   end
   
   z = str2num(thisListitem.getElementsByTagName('z').item(0).getFirstChild.getData);
   if z < minz
       minz = z;
   elseif z > maxz
       maxz = z;
   end  
end

fileID = fopen('ParticleFilterData.txt','r');
formatSpec = '%f %f';
sizeA = [3 Inf];
A = fscanf(fileID,formatSpec,sizeA);
fclose(fileID);
figure;
hold on;
scatter(A(1,2:end),A(2,2:end),A(3,2:end).*200,'blue','filled')
cords = zeros(2,3);
cords(:,1) = A(1:2,1);
cords(:,2) = [A(1,1)+cosd(A(3,1))*fovdistance+tand(30)*fovdistance*sind(A(3,1)) A(2,1)+sind(A(3,1))*fovdistance-tand(30)*fovdistance*sind(90-A(3,1))];
cords(:,3) = [A(1,1)+cosd(A(3,1))*fovdistance-tand(30)*fovdistance*sind(A(3,1)) A(2,1)+sind(A(3,1))*fovdistance+tand(30)*fovdistance*sind(90-A(3,1))];
rel = [0 1 2];
cords = cords';
%cords = cords(:,[1:1-1,2,1+1:2-1,1,2+1:end]);
%triplot(delaunayTriangulation(cords));
linemat(1,:) = A(1:2,1);
linemat(2,:) = [A(1,1)+cosd(A(3,1))*fovdistance A(2,1)+sind(A(3,1)*fovdistance)];
line(linemat(:,1), linemat(:,2),'color','black','linewidth',2)
scatter(A(1,1),A(2,1),150,'red','filled')
axis([minx maxx minz maxz])
axis square
grid on

