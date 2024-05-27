import matplotlib.pyplot as plt
import networkx as nx

# Input data
data = {
    "0x01ca": [("0xb356", 1),("0xd262", 1)],
    "0x305d": [],
    "0x3b62": [],
    "0x475b": [],
    "0x480c": [("0xd262", 1),("0x87df", 1)],
    "0x4a12": [],
    "0x4b8c": [("0x6f6b", 1),("0x6f6b", 1)],
    "0x5214": [],
    "0x575a": [("0xd29a", 1)],
    "0x5afe": [("0x480c", 1),("0x480c", 1)],
    "0x6b62": [("0xc7d1", 1),("0x3b62", 1)],
    "0x6f6b": [("0x5214", 1),("0x5214", 1)],
    "0x7bb6": [],
    "0x87df": [],
    "0x9fd3": [("0x5214", 1),("0x6f6b", 1)],
    "0xb356": [("0xd29a", 1)],
    "0xb5b5": [("0x5214", 1),("0x9fd3", 1)],
    "0xc7d1": [("0x3b62", 1),("0x3b62", 1)],
    "0xd262": [("0x4b8c", 1),("0x3b62", 1)],
    "0xd29a": [("0x3b62", 1)],
    "0xed24": [],
    "0xf376": [],
    "0xf479": [("0x5afe", 1),("0xfc46", 1)],
    "0xf7e6": [("0x6f6b", 1),("0x5214", 1)],
    "0xfc46": [("0x3b62", 1),("0x3b62", 1)],
}


# Create a directed graph
G = nx.DiGraph()

# Add edges to the graph
for node, edges in data.items():
    for edge, weight in edges:
        G.add_edge(edge, node)


# Draw the graph
plt.figure(figsize=(20, 20))  # Increase the figure size for better visibility

# Use Circular layout for better spacing
pos = nx.circular_layout(G)

# Define node colors and sizes
node_colors = ['lightblue' for _ in G.nodes()]
node_sizes = [3000 for _ in G.nodes()]

# Draw nodes and edges
nx.draw(G, pos, with_labels=True, node_size=node_sizes, node_color=node_colors, font_size=10, font_weight='bold')

# Draw all edges except the special one
edges = [e for e in G.edges()]
nx.draw_networkx_edges(G, pos, edgelist=edges, arrows=True)

# Show the plot
plt.title('Graph Visualization with Circular Layout and Highlighted Edge')
plt.show()