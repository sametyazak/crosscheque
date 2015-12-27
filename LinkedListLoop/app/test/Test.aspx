<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="LinkedListLoop.app.test.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../lib/vis/vis.css" rel="stylesheet" />
    <script src="../../lib/vis/vis.js"></script>

    <style type="text/css">
    #mynetwork {
      width: 800px;
      height: 800px;
      border: 1px solid lightgray;
    }
  </style>
</head>
<body>
    <div id="mynetwork">
        <div class="vis-network" tabindex="900" style="position: relative; overflow: hidden; touch-action: none; -webkit-user-select: none; -webkit-user-drag: none; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); width: 100%; height: 100%;">
            <canvas width="800" height="800" style="position: relative; touch-action: none; -webkit-user-select: none; -webkit-user-drag: none; -webkit-tap-highlight-color: rgba(0, 0, 0, 0); width: 100%; height: 100%;"></canvas>
            <div class="vis-network-tooltip" style="left: 357px; top: 140px; visibility: hidden;">Country: Nigeria<br>
                Team: Chelsea</div>
        </div>
    </div>


    <script type="text/javascript">
        var network;
        var allNodes;
        var highlightActive = false;

        var nodes = [
      { id: 1, label: 'Abdelmoumene Djabou', title: 'Country: ' + 'Algeria' + '<br>' + 'Team: ' + 'Club Africain', value: 22, group: 24, x: -1392.5499, y: 1124.1614 },
      { id: 2, label: 'Abel Aguilar', title: 'Country: ' + 'Colombia' + '<br>' + 'Team: ' + 'Toulouse', value: 24, group: 11, x: -660.82574, y: 1009.18976 },
      { id: 3, label: 'Abel Hernández', title: 'Country: ' + 'Uruguay' + '<br>' + 'Team: ' + 'Palermo', value: 22, group: 6, x: -85.6025, y: -6.6782646 },
      { id: 4, label: 'Adam Kwarasey', title: 'Country: ' + 'Ghana' + '<br>' + 'Team: ' + 'Strømsgodset', value: 22, group: 5, x: 427.39853, y: 1398.1719 },
      { id: 5, label: 'Adam Lallana', title: 'Country: ' + 'England' + '<br>' + 'Team: ' + 'Southampton', value: 26, group: 28, x: -133.68427, y: -732.50476 },
      { id: 6, label: 'Adam Taggart', title: 'Country: ' + 'Australia' + '<br>' + 'Team: ' + 'Newcastle Jets', value: 22, group: 12, x: 2042.4272, y: -579.6042 },
      { id: 7, label: 'Admir Mehmedi', title: 'Country: ' + 'Switzerland' + '<br>' + 'Team: ' + 'SC Freiburg', value: 24, group: 0, x: 126.91814, y: 115.84123 },
      { id: 8, label: 'Adnan Januzaj', title: 'Country: ' + 'Belgium' + '<br>' + 'Team: ' + 'Manchester United', value: 34, group: 28, x: -638.503, y: -663.07904 },
      { id: 9, label: 'Adrián Bone', title: 'Country: ' + 'Ecuador' + '<br>' + 'Team: ' + 'El Nacional', value: 22, group: 4, x: -1657.1593, y: -645.2429 }

        ];
        // create an array with edges
        var edges = [
          { from: 1, to: 15 },
          { from: 1, to: 97 },
          { from: 1, to: 108 },
          { from: 1, to: 173 },
          { from: 1, to: 195 },
          { from: 1, to: 205 },
          { from: 1, to: 218 },
          { from: 1, to: 274 },
          { from: 1, to: 296 },
          { from: 1, to: 418 },
          { from: 1, to: 435 },
          { from: 1, to: 493 },
          { from: 1, to: 494 },
          { from: 1, to: 519 },
          { from: 1, to: 525 },
          { from: 1, to: 526 },
          { from: 1, to: 582 },
          { from: 1, to: 585 },
          { from: 1, to: 605 },
          { from: 1, to: 631 },
          { from: 1, to: 655 },
          { from: 1, to: 722 },
          { from: 2, to: 10 },
          { from: 2, to: 31 },
          { from: 2, to: 96 },
          { from: 2, to: 99 },
          { from: 2, to: 100 },
          { from: 2, to: 105 },
          { from: 2, to: 106 },
          { from: 2, to: 130 },
          { from: 2, to: 153 },
          { from: 2, to: 181 },
          { from: 2, to: 219 },
          { from: 2, to: 234 },
          { from: 2, to: 304 },
          { from: 2, to: 309 },
          { from: 2, to: 322 },
          { from: 2, to: 366 },
          { from: 2, to: 369 },
          { from: 2, to: 370 },
          { from: 2, to: 457 },
          { from: 2, to: 554 },
          { from: 2, to: 630 },
          { from: 2, to: 639 },
          { from: 2, to: 672 },
          { from: 2, to: 701 },
          { from: 3, to: 38 },
          { from: 3, to: 39 },
          { from: 3, to: 121 },
          { from: 3, to: 129 },
          { from: 3, to: 165 },
          { from: 3, to: 166 },
          { from: 3, to: 167 },
          { from: 3, to: 168 },
          { from: 3, to: 185 },
          { from: 3, to: 191 },
          { from: 3, to: 226 },
          { from: 3, to: 240 },
          { from: 3, to: 352 },
          { from: 3, to: 359 },
          { from: 3, to: 430 },
          { from: 3, to: 461 },
          { from: 3, to: 463 },
          { from: 3, to: 486 },
          { from: 3, to: 531 },
          { from: 3, to: 607 },
          { from: 3, to: 634 },
          { from: 3, to: 711 },
          { from: 4, to: 11 },
          { from: 4, to: 18 },
          { from: 4, to: 43 },
          { from: 4, to: 65 },
          { from: 4, to: 118 },
          { from: 4, to: 138 },
          { from: 4, to: 199 },
          { from: 4, to: 220 },
          { from: 4, to: 272 },
          { from: 4, to: 340 },
          { from: 4, to: 346 },
          { from: 4, to: 347 },
          { from: 4, to: 387 }
        ];


        var nodesDataset = new vis.DataSet(nodes); // these come from WorldCup2014.js
        var edgesDataset = new vis.DataSet(edges); // these come from WorldCup2014.js

        function redrawAll() {
            var container = document.getElementById('mynetwork');
            var options = {

                nodes: {
                    shape: 'dot',
                    scaling: {
                        label: {
                            min: 8,
                            max: 20
                        }
                    },
                    shadow: {
                        "enabled": true
                    }
                },

                "edges": {
                    "arrows": {
                        "to": {
                            "enabled": true
                        }
                    },
                    "scaling": {
                        "min": 2,
                        "max": 10
                    },
                    "shadow": {
                        "enabled": true
                    },
                    "smooth": {
                        "type": 'continuous'
                    }
                },
                physics: {
                    stabilization: {
                        enabled: true,
                        iterations: 2000,
                        updateInterval: 25
                    }
                }
            };
            var data = { nodes: nodesDataset, edges: edgesDataset } // Note: data is coming from ./datasources/WorldCup2014.js


            network = new vis.Network(container, data, options);

            // get a JSON object
            allNodes = nodesDataset.get({ returnType: "Object" });

            network.on("click", neighbourhoodHighlight);
        }

        function neighbourhoodHighlight(params) {
            // if something is selected:
            if (params.nodes.length > 0) {
                highlightActive = true;
                var i, j;
                var selectedNode = params.nodes[0];
                var degrees = 2;

                // mark all nodes as hard to read.
                for (var nodeId in allNodes) {
                    allNodes[nodeId].color = 'rgba(200,200,200,0.5)';
                    if (allNodes[nodeId].hiddenLabel === undefined) {
                        allNodes[nodeId].hiddenLabel = allNodes[nodeId].label;
                        allNodes[nodeId].label = undefined;
                    }
                }
                var connectedNodes = network.getConnectedNodes(selectedNode);
                var allConnectedNodes = [];

                // get the second degree nodes
                for (i = 1; i < degrees; i++) {
                    for (j = 0; j < connectedNodes.length; j++) {
                        allConnectedNodes = allConnectedNodes.concat(network.getConnectedNodes(connectedNodes[j]));
                    }
                }

                // all second degree nodes get a different color and their label back
                for (i = 0; i < allConnectedNodes.length; i++) {
                    allNodes[allConnectedNodes[i]].color = 'rgba(150,150,150,0.75)';
                    if (allNodes[allConnectedNodes[i]].hiddenLabel !== undefined) {
                        allNodes[allConnectedNodes[i]].label = allNodes[allConnectedNodes[i]].hiddenLabel;
                        allNodes[allConnectedNodes[i]].hiddenLabel = undefined;
                    }
                }

                // all first degree nodes get their own color and their label back
                for (i = 0; i < connectedNodes.length; i++) {
                    allNodes[connectedNodes[i]].color = undefined;
                    if (allNodes[connectedNodes[i]].hiddenLabel !== undefined) {
                        allNodes[connectedNodes[i]].label = allNodes[connectedNodes[i]].hiddenLabel;
                        allNodes[connectedNodes[i]].hiddenLabel = undefined;
                    }
                }

                // the main node gets its own color and its label back.
                allNodes[selectedNode].color = undefined;
                if (allNodes[selectedNode].hiddenLabel !== undefined) {
                    allNodes[selectedNode].label = allNodes[selectedNode].hiddenLabel;
                    allNodes[selectedNode].hiddenLabel = undefined;
                }
            }
            else if (highlightActive === true) {
                // reset all nodes
                for (var nodeId in allNodes) {
                    allNodes[nodeId].color = undefined;
                    if (allNodes[nodeId].hiddenLabel !== undefined) {
                        allNodes[nodeId].label = allNodes[nodeId].hiddenLabel;
                        allNodes[nodeId].hiddenLabel = undefined;
                    }
                }
                highlightActive = false
            }

            // transform the object into an array
            var updateArray = [];
            for (nodeId in allNodes) {
                if (allNodes.hasOwnProperty(nodeId)) {
                    updateArray.push(allNodes[nodeId]);
                }
            }
            nodesDataset.update(updateArray);
        }

        redrawAll()

</script>
</body>
</html>
