import matplotlib.pyplot as plt
import networkx as nx

# Define the input data for CEX nodes
data = {
    "CEX 0x1c22": [("0x305d", 8),("0x3b62", 1)],
    "CEX 0x2b14": [("0x3b62", 14),("0x575a", 1)],
    "CEX 0x62f4": [("0x3b62", 11),("0xb356", 1),("0x575a", 1)],
    "CEX 0x9a2d": [("0xf376", 6),("0x6f6b", 1)],
    "CEX 0xa446": [("0x475b", 6),("0xfc46", 1)],
    "CEX 0xa712": [("0x3b62", 26),("0x01ca", 1),("0xfc46", 1)],
    "CEX 0xa886": [("0x3b62", 29),("0xd262", 1),("0x4b8c", 1),("0xed24", 3),("0xf479", 6),("0xfc46", 2),("0xc7d1", 1),("0xb356", 1),("0xd29a", 2),("0xb5b5", 2),("0x87df", 4),("0x480c", 1),("0x5afe", 1),("0x6b62", 1),("0x01ca", 1)],
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
plt.figure(figsize=(30, 30))  # Increase the figure size for better visibility

# Use circular layout for better spacing
pos = nx.circular_layout(G)

# Define node colors, making CEX nodes a different color
node_colors = []
node_sizes = []
for node in G.nodes():
    if node.startswith("CEX") or node.startswith("Distribution"):
        node_colors.append('red')
        node_sizes.append(5000)
    else:
        node_colors.append('lightblue')
        node_sizes.append(3000)

# Draw nodes and edges
nx.draw(G, pos, with_labels=True, node_size=node_sizes, node_color=node_colors, font_size=10, font_weight='bold', arrows=False)
edge_labels = nx.get_edge_attributes(G, 'weight')

# Draw all edges
nx.draw_networkx_edges(G, pos, edgelist=G.edges(), arrows=False)

# Draw edge labels
# nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_labels)

plt.title('Graph Visualization of CEX Nodes')
plt.show()