import matplotlib.pyplot as plt
import networkx as nx

# Combined input data including previous nodes and CEX nodes
data = {
    "0x01ca": [("0x3b62", 3),("0x6f6b", 3),("0xb356", 1),("0xd262", 1),("0x87df", 1)],
    "0x305d": [("0x4a12", 1),("0x475b", 1)],
    "0x3b62": [("0x475b", 1),("0xd29a", 14),("0xf7e6", 2),("0xd262", 3),("0x01ca", 3),("0xc7d1", 14),("0xb356", 19),("0xf479", 1),("0x6b62", 1),("0x5afe", 1),("0x480c", 2),("0x87df", 2),("0xb5b5", 4),("0xfc46", 1),("0x9fd3", 1),("0x6f6b", 1)],
    "0x475b": [("0x3b62", 1),("0x305d", 1)],
    "0x480c": [("0x5afe", 4),("0xfc46", 1),("0x87df", 2),("0xf479", 1),("0x3b62", 2),("0xd262", 2)],
    "0x4a12": [("0x305d", 1)],
    "0x4b8c": [("0xd262", 1),("0xb356", 2),("0xf479", 1),("0x6f6b", 2),("0x87df", 1),("0x9fd3", 1)],
    "0x575a": [("0xd29a", 1)],
    "0x5afe": [("0x6b62", 1),("0xd29a", 2),("0x480c", 4),("0x3b62", 1),("0xf479", 1)],
    "0x6b62": [("0xed24", 2),("0xc7d1", 2),("0x5afe", 1),("0x3b62", 1)],
    "0x6f6b": [("0x01ca", 3),("0x9fd3", 2),("0x4b8c", 2),("0xf7e6", 1),("0x3b62", 1)],
    "0x7bb6": [],
    "0x87df": [("0x480c", 2),("0xd29a", 1),("0xb5b5", 1),("0x3b62", 2),("0x01ca", 1),("0x4b8c", 1)],
    "0x9fd3": [("0xb5b5", 2),("0xd262", 1),("0x6f6b", 2),("0x3b62", 1),("0x4b8c", 1)],
    "0xb356": [("0xf479", 2),("0xc7d1", 2),("0xd29a", 9),("0x01ca", 1),("0xd262", 1),("0x4b8c", 2),("0x3b62", 19),("0xfc46", 1)],
    "0xb5b5": [("0xd29a", 2),("0x87df", 1),("0x9fd3", 2),("0x3b62", 4)],
    "0xc7d1": [("0x6b62", 2),("0x3b62", 14),("0xb356", 2),("0xed24", 1),("0xd29a", 1),("0xfc46", 1)],
    "0xd262": [("0x3b62", 3),("0x9fd3", 1),("0x01ca", 1),("0xb356", 1),("0x4b8c", 1),("0x480c", 2)],
    "0xd29a": [("0x3b62", 14),("0x575a", 1),("0xb356", 9),("0xed24", 1),("0x5afe", 2),("0x87df", 1),("0xb5b5", 2),("0xc7d1", 1),("0xfc46", 1)],
    "0xed24": [("0x6b62", 2),("0xd29a", 1),("0xc7d1", 1)],
    "0xf376": [],
    "0xf479": [("0xb356", 2),("0x4b8c", 1),("0xfc46", 2),("0x3b62", 1),("0x480c", 1),("0x5afe", 1)],
    "0xf7e6": [("0x3b62", 2),("0x6f6b", 1)],
    "0xfc46": [("0x480c", 1),("0xf479", 2),("0xc7d1", 1),("0xb356", 1),("0xd29a", 1),("0x3b62", 3)],
    "CEX 0x1c22": [("0x305d", 8),("0x3b62", 1)],
    "CEX 0x2b14": [("0x3b62", 14),("0x575a", 1)],
    "CEX 0x62f4": [("0x3b62", 11),("0xb356", 1),("0x575a", 1)],
    "CEX 0x9a2d": [("0xf376", 6),("0x6f6b", 1)],
    "CEX 0xa446": [("0x475b", 6),("0xfc46", 1)],
    "CEX 0xa712": [("0x3b62", 26),("0x01ca", 1),("0xfc46", 1),("0x5214", 2)],
    "CEX 0xa886": [("0x3b62", 29),("0xd262", 1),("0x4b8c", 1),("0xed24", 3),("0xf479", 6),("0xfc46", 2),("0xc7d1", 1),("0xb356", 1),("0xd29a", 2),("0xb5b5", 2),("0x87df", 4),("0x480c", 1),("0x5afe", 1),("0x6b62", 1),("0x01ca", 1),("0x5214", 4)],
    "CEX 0xc289": [("0x4a12", 7),("0x87df", 1)],
    "CEX 0xcbbf": [("0x3b62", 17),("0xd29a", 2)],
    "CEX 0xf5a8": [("0xd29a", 9),("0x3b62", 1)],
    "CEX 0xf963": [("0x7bb6", 7),("0x01ca", 1)]
}

# Create a directed graph
G = nx.DiGraph()

# Add edges to the graph
for node, edges in data.items():
    for edge, weight in edges:
        G.add_edge(node, edge, weight=weight)

# Draw the graph
plt.figure(figsize=(40, 40))  # Increase the figure size for better visibility

# Use shell layout for better spacing and organize nodes in concentric circles
pos = nx.shell_layout(G)

# Define node colors and sizes, making CEX nodes a different color and larger
node_colors = []
node_sizes = []
for node in G.nodes():
    if node.startswith("CEX"):
        node_colors.append('red')
        node_sizes.append(5000)
    else:
        node_colors.append('lightblue')
        node_sizes.append(3000)

# Draw nodes and edges
nx.draw(G, pos, with_labels=True, node_size=node_sizes, node_color=node_colors, font_size=10, font_weight='bold')
edge_labels = nx.get_edge_attributes(G, 'weight')

# Draw all edges
nx.draw_networkx_edges(G, pos, edgelist=G.edges(), arrows=True)

# Draw edge labels
#nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_labels)

plt.title('Combined Graph Visualization of Nodes and CEXes')
plt.show()